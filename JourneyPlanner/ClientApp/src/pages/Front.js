import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import Axios from "axios";

import PageContainer from "../components/PageContainer";
import JourneyForm from "../components/JourneyForm";
import MapViewer from "../components/MapViewer";

import { selectUserInput } from "../reducers/route";
import { selectUser } from "../reducers/user";
import SavedJourneys from "../components/SavedJourneys";

function Front() {
  const userInput = useSelector(selectUserInput);
  const user = useSelector(selectUser);
  const [newZoom, setNewZoom] = useState();

  useEffect(() => {
    if (userInput.stops[0] != null && userInput.stops[1] != null) {
      Axios.post("/api/v1/stations/distance", [
        userInput.stops[0],
        userInput.stops[1],
      ]).then((response) => setNewZoom(response.data));
    }
  }, [userInput]);

  const center = () => {
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

  return (
      <>
        <PageContainer>
          <JourneyForm />
          <MapViewer
            stations={userInput.stops}
            lines={[]}
            center={center()}
            zoom={zoom()}
          />
          {
            user === undefined || user.username === null ?
            '' : <SavedJourneys/>
          }
        </PageContainer>
      </>
  );
}

export default Front;
