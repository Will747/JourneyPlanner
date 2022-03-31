import React, { useState, useEffect } from "react";

import { Map, Marker, GoogleApiWrapper, Polyline } from "google-maps-react";
import { Grid, Paper } from "@material-ui/core";

import classes from "./MapViewer.module.css";

function MapViewer({google, stations, lines, handleMarker, center, zoom}) {
  const [render, setRender] = useState();
  useEffect(() => {
    setRender(Math.random());
  }, [])

  return (
    <Grid item xs={12} sm className={classes.Grid}>
      <Paper className={classes.Paper}>
        <div className={classes.MapContainer}>
          <Map
            className={classes.Map}
            google={google}
            zoom={zoom}
            center={center == null ? { lat: 54.2808, lng: -5.4051 } : center}
            randomState={render}
          >
            {stations.map((station, index) => (
              station == null ? '' : 
              <Marker
                key={"station" + index}
                position={{ lat: station.latitude, lng: station.longitude }}
                title={station.codeName}
                onClick={handleMarker}
              />
            ))}
            {lines.map((line, index) => (
              line == null ? '' :
              <Polyline
              key={line.from + index}
              path={line.coordinates}
              strokeColor="#eb8500"
              strokeOpacity={0.8}
              strokeWeight={2}
              >
              </Polyline>
            ))}
          </Map>
        </div>
      </Paper>
    </Grid>
  );
}

export default GoogleApiWrapper({
  apiKey: "xxxxxxxxxxxxxxxx",
})(MapViewer);
