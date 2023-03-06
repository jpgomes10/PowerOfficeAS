import { ResponsiveBar } from "@nivo/bar";
import { useTheme, Box } from "@mui/material";

const BarChart = ({ organizationStats }) => {
  const theme = useTheme();
  const colorsTheme = theme.palette;
  const colors = [
    theme.palette.secondary[500],
    theme.palette.secondary[300],
    theme.palette.secondary[300],
    theme.palette.secondary[500],
    theme.palette.secondary[700],
  ];
  const data = organizationStats.map(({ organisasjonsform, count }, i) => ({
    id: organisasjonsform,
    value: count,
    color: colors[i % colors.length],
  }));

  return (
    <Box height={"100%"} width={"100%"} position="relative">
      <ResponsiveBar
        theme={{
          axis: {
            domain: {
              line: {
                stroke: colorsTheme.grey[100],
              },
            },
            legend: {
              text: {
                fill: colorsTheme.grey[100],
              },
            },
            ticks: {
              line: {
                stroke: colorsTheme.grey[100],
                strokeWidth: 1,
              },
              text: {
                fill: colorsTheme.grey[100],
              },
            },
          },
          legends: {
            text: {
              fill: colorsTheme.grey[100],
            },
          },
          tooltip: {
            container: {
              background: colorsTheme.primary[400],
              color: colorsTheme.grey[100],
            },
          },
        }}
        data={data}
        keys={["value"]}
        indexBy="id"
        margin={{ top: 40, right: 80, bottom: 80, left: 80 }}
        padding={0.3}
        colors={({ value, data }) => data.color}
        borderRadius={3}
        borderWidth={1}
        borderColor={{
          from: "color",
          modifiers: [["darker", 0.2]],
        }}
        axisBottom={{
          tickSize: 5,
          tickPadding: 5,
          tickRotation: 0,
          legend: "Organisasjonsform",
          legendPosition: "middle",
          legendOffset: 50,
        }}
        axisLeft={{
          tickSize: 5,
          tickPadding: 5,
          tickRotation: 0,
          legend: "Count",
          legendPosition: "middle",
          legendOffset: -50,
        }}
        labelSkipWidth={12}
        labelSkipHeight={12}
        labelTextColor={{
          from: "color",
          modifiers: [["darker", 10]],
        }}
        animate={true}
        motionStiffness={90}
        motionDamping={15}
      />
    </Box>
  );
};

export default BarChart;
