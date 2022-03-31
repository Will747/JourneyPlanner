import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import axios from "axios";

import {
  selectUserInput,
  overwriteRoutes,
  selectRoutes,
} from "../reducers/route";

import Snackbar from "@material-ui/core/Snackbar";
import SaveIcon from "@material-ui/icons/Save";
import IconButton from "@material-ui/core/IconButton";
import Tooltip from "@material-ui/core/Tooltip";
import MuiAlert from "@material-ui/lab/Alert";
import CircularProgress from "@material-ui/core/CircularProgress";
import Checkbox from "@material-ui/core/Checkbox";
import FormControlLabel from "@material-ui/core/FormControlLabel";

import RoutesBox from "../components/routes/RoutesBox";
import PageContainer from "../components/PageContainer";
import MapViewer2 from "../components/MapViewer2";
import DirectServices from "../components/routes/DirectServices";

function PathView() {
  const dispatch = useDispatch();
  const userInput = useSelector(selectUserInput);
  const routes = useSelector(selectRoutes);
  const [newZoom, setNewZoom] = useState();
  const [mapMarker, setMapMarker] = useState(false);
  const [savedAlert, setSavedAlert] = useState(false);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    //Needs the distance between the first and last station for the map zoom
    if (userInput.stops[0] != null && userInput.stops[1] != null) {
      axios
        .post("/api/v1/stations/distance", [
          userInput.stops[0],
          userInput.stops[1],
        ])
        .then((response) => setNewZoom(response.data));
    }

    //Fetch path
    axios.post("/api/v1/stations/route", userInput.stops).then((response) => {
      dispatch(overwriteRoutes(response.data));
      setLoading(false);
    });
  }, [userInput, newZoom, dispatch]);

  const center = () => {
    //Returns the coordinates the map should center at
    if (userInput.stops[0] == null) {
      return { lat: 54.2808, lng: -5.4051 };
    } else if (userInput.stops[1] == null) {
      return {
        lat: userInput.stops[0].latitude,
        lng: userInput.stops[0].longitude,
      };
    } else {
      return {
        lat: (userInput.stops[0].latitude + userInput.stops[1].latitude) / 2,
        lng: (userInput.stops[0].longitude + userInput.stops[1].longitude) / 2,
      };
    }
  };

  const zoom = () => {
    if (userInput.stops[0] == null) {
      return 5;
    } else if (userInput.stops[1] == null) {
      return 12;
    } else {
      return (-400 / 81159) * newZoom + 8.16;
    }
  };

  const handleSaveJourney = () => {
    let type;
    switch (userInput.type) {
      case "Single":
        type = 1;
        break;
      case "Return":
        type = 2;
        break;
      case "Multi-Stop":
        type = 3;
        break;
    }

    let algorithm = 1;
    switch (userInput.algorithm) {
      case "Distance":
        algorithm = 1;
        break;
      case "Timetable":
        algorithm = 2;
        break;
    }

    const routeData = {
      Stops: userInput.stops,
      Type: type,
      AlgorithmType: algorithm,
    };

    axios.post("/api/v1/user/routes", routeData).then(setSavedAlert(true));
  };

  if (loading) {
    return (
      <PageContainer>
        <CircularProgress />
      </PageContainer>
    );
  } else {
    return (
      <PageContainer>
        <RoutesBox />

        {routes.length < 1 || routes[0]._stations == null ? (
          ""
        ) : (
          <>
            <MapViewer2 routes={routes} zoom={zoom()} center={center()} showMarkers={mapMarker} />

            <DirectServices />

            <Tooltip title="Save Journey">
              <IconButton onClick={handleSaveJourney}>
                <SaveIcon />
              </IconButton>
            </Tooltip>

            <FormControlLabel
              control={
                <Checkbox
                  checked={mapMarker}
                  onChange={event => setMapMarker(event.target.checked)}
                />
              }
              label="Show map markers"
            />

            <Snackbar
              open={savedAlert}
              autoHideDuration={6000}
              onClose={(event) => setSavedAlert(false)}
            >
              <MuiAlert>Successfully Saved Journey</MuiAlert>
            </Snackbar>
          </>
        )}
      </PageContainer>
    );
  }
}

export default PathView;
