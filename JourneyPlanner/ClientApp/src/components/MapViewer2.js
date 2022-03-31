import React, { useState, useEffect } from "react";

import { Map, Marker, GoogleApiWrapper, Polyline } from "google-maps-react";
import { Grid, Paper } from "@material-ui/core";

import classes from "./MapViewer.module.css";

function MapViewer2({
  google,
  showMarkers,
  center,
  zoom,
  routes,
}) {
  const [render, setRender] = useState();
  useEffect(() => {
    setRender(Math.random());
  }, []);

  const getRouteColour = (index) => {
    switch (index) {
      case 0:
        return "#eb8500";
      case 1:
        return "#004fa8";
      case 2:
        return "#9e9400";
      case 3:
        return "#9e1d00";
      case 4:
        return "#00919e";
      case 5:
        return "#e30000";
      case 6:
        return "#9b00e3";
    }
  };

  return (
    <Grid item>
      <Paper className={classes.Paper}>
        <div className={classes.MapContainer}>
          <Map
            className={classes.Map}
            google={google}
            zoom={zoom}
            center={center == null ? { lat: 54.2808, lng: -5.4051 } : center}
            randomState={render}
          >

            {routes.map((route, routeIndex) => {
              return (
                route.lines.map((line, index) =>
                  line == null ? (
                    ""
                  ) : (
                    <Polyline
                      key={line.from + index}
                      path={line.coordinates}
                      strokeColor={getRouteColour(routeIndex)}
                      strokeOpacity={0.8}
                      strokeWeight={2}
                    ></Polyline>
                  )
                ));
            })}
                
            {showMarkers ? routes.map((route, routeIndex) => {
              return (
                route._stations.map((station, index) => (
                  station == null ? '' : 
                  <Marker
                    key={"station" + index}
                    position={{ lat: station.latitude, lng: station.longitude }}
                    title={station.codeName}
                  />
                ))
                )
            }) : ''}

          </Map>
        </div>
      </Paper>
    </Grid>
  );
}

export default GoogleApiWrapper({
  apiKey: "xxxxxxxxxxxx",
})(MapViewer2);
