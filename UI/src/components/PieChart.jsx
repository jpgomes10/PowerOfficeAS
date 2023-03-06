import { ResponsivePie } from "@nivo/pie";
import { useTheme, Box } from "@mui/material";

const PieChart = ({ organizationStats }) => {
  const theme = useTheme();
  const colorsTheme = theme.palette;
  const colors = [
    theme.palette.secondary[500],
    theme.palette.secondary[300],
    theme.palette.secondary[300],
    theme.palette.secondary[500],
    theme.palette.secondary[700],
  ];
  const data = organizationStats.map(
    ({ organisasjonsform, percentage }, i) => ({
      id: organisasjonsform,
      label: organisasjonsform,
      value: percentage,
      color: colors[i % colors.length],
    })
  );

  return (
    <Box
      height={"100%"}
      width={"100%"}
      minHeight={undefined}
      minWidth={undefined}
      position="relative"
    >
      <ResponsivePie
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
        margin={{ top: 40, right: 80, bottom: 80, left: 80 }}
        innerRadius={0.5}
        padAngle={0.7}
        cornerRadius={3}
        activeOuterRadiusOffset={8}
        borderWidth={1}
        borderColor={{
          from: "color",
          modifiers: [["darker", 0.2]],
        }}
        arcLinkLabelsSkipAngle={10}
        arcLinkLabelsTextColor={colorsTheme.grey[100]}
        arcLinkLabelsThickness={2}
        arcLinkLabelsColor={{ from: "color" }}
        arcLabelsSkipAngle={10}
        arcLabelsTextColor={{
          from: "color",
          modifiers: [["darker", 2]],
        }}
        defs={[
          {
            id: "dots",
            type: "patternDots",
            background: "inherit",
            color: "rgba(255, 255, 255, 0.3)",
            size: 4,
            padding: 1,
            stagger: true,
          },
          {
            id: "lines",
            type: "patternLines",
            background: "inherit",
            color: "rgba(255, 255, 255, 0.3)",
            rotation: -45,
            lineWidth: 6,
            spacing: 10,
          },
        ]}
        legends={[
          {
            anchor: "bottom",
            direction: "row",
            justify: false,
            translateX: 0,
            translateY: 56,
            itemsSpacing: 0,
            itemWidth: 100,
            itemHeight: 18,
            itemTextColor: colorsTheme.grey[100],
            itemDirection: "left-to-right",
            itemOpacity: 1,
            symbolSize: 18,
            symbolShape: "circle",
            effects: [
              {
                on: "hover",
                style: {
                  itemTextColor: colorsTheme.primary[400],
                },
              },
            ],
          },
        ]}
      />
    </Box>
  );
};

export default PieChart;
