import React, { useState } from "react";
import TabBar from "~/pageList/components/tapbar";
import TabContent from "~/pageList/components/tab-content";

const TabBarContainer: React.FC = () => {
  const [activeTab, setActiveTab] = useState("active");

  const onSelectTab = (tabName: string) => {
    setActiveTab(tabName);
  };

  return (
    <div>
      <TabBar activeTab={activeTab} onSelectTab={onSelectTab} />
      <TabContent activeTab={activeTab} />
    </div>
  );
};

export default TabBarContainer;
