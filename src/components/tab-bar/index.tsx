import React, { useState } from "react";
import { AppBar, Tab, Tabs } from "@mui/material";

type TabBarItem = {
  label: string;
  content: React.ReactNode;
};

type TabBarProps = {
  items: TabBarItem[];
};

const TabBar: React.FC<TabBarProps> = ({ items }) => {
  const [activeTab, setActiveTab] = useState(0);

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setActiveTab(newValue);
  };

  return (
    <>
      <AppBar
        position="static"
        sx={{ marginBottom: "20px", backgroundColor: "white", boxShadow: "none", minHeight: "auto" }}
      >
        <Tabs value={activeTab} onChange={handleTabChange} sx={{ minHeight: "auto" }}>
          {items.map((item, index) => (
            <Tab
              key={index}
              label={item.label}
              sx={{
                textTransform: "none",
                fontSize: "12px",
                lineHeight: "14px",
                fontWeight: activeTab === index ? "bold" : "normal",
                padding: "0",
                minHeight: "auto",
                paddingBottom: "3px",
                marginRight: "20px",
                minWidth: "auto",
                color: "#00305C",
              }}
            />
          ))}
        </Tabs>
      </AppBar>
      <>{items[activeTab]?.content}</>
    </>
  );
};

export default TabBar;
