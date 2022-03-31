import React, { useState } from "react";
import { useSelector } from "react-redux";

import { selectRoutes } from "../../reducers/route";

import { Stepper, Step, StepLabel } from '@material-ui/core';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';

import Tile from "../Tile";
import classes from './RoutesBox.module.css';

function RoutesBox() {
  const routes = useSelector(selectRoutes);
  const [pathIndex, setPathIndex] = useState(0);

  const handleChange = (event, value) => {
    setPathIndex(value);
  }
    return (
      <Tile>
        {routes.length < 1 || routes[0]._stations == null ? (
          <h3>No Route Found</h3>
        ) : (
          <>
          <strong>Stops:</strong>
  
          {routes.length > 1 ? 
          <Tabs value={pathIndex} onChange={handleChange} variant="scrollable"
          scrollButtons="auto">
            {routes.map((path, index) => 
            <Tab key={"Path " + (index + 1)} label={"Path " + (index + 1)} />
            )}
          </Tabs> : ""}
          
  
              <Stepper className={classes.List} activeStep={0} orientation="vertical">
              {routes[pathIndex]._stations.map((station) =>
                  <Step key={"station" + station.codeName}>
                   <StepLabel>{station.name}</StepLabel>
                  </Step>
              )}
              </Stepper>
          </>
        )}
      </Tile>
    );
}

export default RoutesBox;
