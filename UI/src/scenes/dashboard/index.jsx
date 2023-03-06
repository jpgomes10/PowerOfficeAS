import { postOrganizations, postOrganizationStats } from "state/api";
import React, { useState } from "react";
import FlexBetween from "components/FlexBetween";
import PieChart from "components/PieChart";
import BarChart from "components/BarChart";
import {
  useTheme,
  Box,
  useMediaQuery,
  Backdrop,
  CircularProgress,
  Snackbar,
} from "@mui/material";
import Header from "components/Header";
import FileChooser from "components/FileChooser";

const Dashboard = () => {
  const theme = useTheme();
  const [organizations, setOrganizations] = useState([]);
  const [organizationStats, setOrganizationStats] = useState([]);
  const [loading, setLoading] = useState(false);
  const isNonMediumScreens = useMediaQuery("(min-width: 1200px)");
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState("");

  const handleGetOrganizations = async (file) => {
    try {
      setLoading(true);
      const formData = new FormData();
      formData.append("file", file);
      const organizationsInfoStream = await postOrganizations(formData);
      if (organizationsInfoStream.status === 400) {
        setSnackbarMessage("Invalid CSV file format");
        setSnackbarOpen(true);
        return;
      }
      setOrganizations(organizationsInfoStream);

      await organizationsInfoStream.blob().then(async (blob) => {
        const formDataStats = new FormData();
        formDataStats.append("file", blob);
        const data = await postOrganizationStats(formDataStats);
        setOrganizationStats(data);
      });
    } catch (error) {
      setSnackbarMessage("Invalid CSV file format");
      setSnackbarOpen(true);
    } finally {
      setLoading(false);
    }
  };

  const handleSnackbarClose = () => {
    setSnackbarOpen(false);
  };

  return (
    <Box m="1.5rem 2.5rem">
      <Snackbar
        open={snackbarOpen}
        message={snackbarMessage}
        autoHideDuration={3000}
        onClose={handleSnackbarClose}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
        sx={{
          "& .MuiPaper-root": {
            backgroundColor: "darkRed",
            fontSize: "1.2rem",
          },

          display: "flex",
          alignItems: "center",
          justifyContent: "center",
          width: "100%",
        }}
      />
      <FlexBetween>
        <Header title="DASHBOARD" subtitle="Welcome to your dashboard" />
        <Box>
          <FileChooser handleGetOrganizations={handleGetOrganizations} />
        </Box>
      </FlexBetween>
      <Box
        mt="20px"
        display="grid"
        gridTemplateColumns="repeat(12, 1fr)"
        gridAutoRows="160px"
        gap="20px"
        sx={{
          "& > div": { gridColumn: isNonMediumScreens ? undefined : "span 12" },
        }}
      >
        <Box gridRow="span 4" gridColumn="span 6">
          {organizationStats.length > 0 && (
            <PieChart organizationStats={organizationStats} />
          )}
        </Box>
        <Box gridRow="span 4" gridColumn="span 6">
          {organizationStats.length > 0 && (
            <BarChart organizationStats={organizationStats} />
          )}
        </Box>
      </Box>
      <Backdrop
        sx={{
          zIndex: (theme) => theme.zIndex.drawer + 1,
          color: theme.palette.grey,
        }}
        open={loading}
      >
        <CircularProgress color="primary" size={60} />
      </Backdrop>
    </Box>
  );
};

export default Dashboard;
