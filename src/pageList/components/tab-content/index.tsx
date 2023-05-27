import React from "react";
import TableActive from "~/pageList/components/table/active";
import TableSign from "~/pageList/components/table/sign";
import TableHistory from "~/pageList/components/table/history";
import { CSSTransition } from "react-transition-group";

interface TabContentProps {
  activeTab: string;
}

const TabContent: React.FC<TabContentProps> = ({ activeTab }) => {
  return (
    <>
      <div>
        <CSSTransition in={activeTab === "active"} timeout={300} classNames="fade">
          <div>{activeTab === "active" && <TableActive />}</div>
        </CSSTransition>
        <CSSTransition in={activeTab === "sent"} timeout={300} classNames="fade">
          <div>{activeTab === "sent" && <TableSign />}</div>
        </CSSTransition>
        <CSSTransition in={activeTab === "history"} timeout={300} classNames="fade">
          <div>{activeTab === "history" && <TableHistory />}</div>
        </CSSTransition>
      </div>
      <style>
        {`
          .fade-enter {
            opacity: 0.01;
          }

          .fade-enter.fade-enter-active {
            opacity: 1;
            transition: opacity 300ms ease-in-out;
          }

          .fade-exit {
            opacity: 1;
          }

          .fade-exit.fade-exit-active {
            opacity: 0.01;
            transition: opacity 300ms ease-in-out;
          }
        `}
      </style>
    </>
  );
};

export default TabContent;
