import "./App.css";
import { Router, Switch } from "react-router-dom";
import React from "react";
import "antd/dist/antd.css";
import history from "./router/History";
import MainLayout from "./components/main-layout/MainLayout";

function App() {
  return (
    <div className="App">
      <Router history={history}>
        <div>
          <Switch>
            <MainLayout />
          </Switch>
        </div>
      </Router>
    </div>
  );
}

export default App;
