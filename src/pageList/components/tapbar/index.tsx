import React, { CSSProperties } from "react";
import { Nav, Tab } from "react-bootstrap";

interface TabBarProps {
  activeTab: string;
  onSelectTab: (tabName: string) => void;
}

const TabBar: React.FC<TabBarProps> = ({ activeTab, onSelectTab }) => {
  return (
    <>
      <Tab.Container activeKey={activeTab} onSelect={tabName => onSelectTab(tabName! as string)}>
        <Nav variant="pills">
          <Nav.Item>
            <Nav.Link eventKey="active">Active</Nav.Link>
          </Nav.Item>
          <Nav.Item>
            <Nav.Link eventKey="sent">Semnate</Nav.Link>
          </Nav.Item>
          <Nav.Item>
            <Nav.Link eventKey="history">Istoric</Nav.Link>
          </Nav.Item>
        </Nav>
      </Tab.Container>
      <style>
        {`
          .nav-pills {
            margin-left: 20px;
            display: flex;
            font-size: 12px;
            margin-bottom: 50px;
          }

          .nav-item a {
            margin-right: 20px;
            color: rgba(0, 48, 92, 1);
            text-decoration: none;
          }

          .nav-link.active {
            font-weight: bold;
            border-bottom: 2px solid rgba(0, 48, 92, 1);
          }
        `}
      </style>
    </>
  );
};

export default TabBar;
