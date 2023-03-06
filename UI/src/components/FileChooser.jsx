import React, { useState } from "react";
import { Box, TextField, Button, InputAdornment } from "@mui/material";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";

function FileChooser({ handleGetOrganizations }) {
  const [file, setFile] = useState(null);

  const handleFileUpload = async (event) => {
    const file = event.target.files[0];
    setFile(file);
  };

  const handleUploadButtonClick = () => {
    handleGetOrganizations(file);
  };

  return (
    <Box>
      <TextField
        type="file"
        variant="outlined"
        size="small"
        onChange={handleFileUpload}
        InputProps={{
          endAdornment: (
            <InputAdornment position="end">
              <Button
                variant="contained"
                color="primary"
                onClick={handleUploadButtonClick}
                startIcon={<CloudUploadIcon />}
                disabled={!file}
              >
                Upload
              </Button>
            </InputAdornment>
          ),
        }}
      />
    </Box>
  );
}

export default FileChooser;
