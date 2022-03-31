import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { useHistory } from "react-router-dom";

import { Button } from "@material-ui/core";

import Type from "./JourneyForm/Type";
import Single from "./JourneyForm/Single";
import Return from "./JourneyForm/Return";
import MultiStop from "./JourneyForm/MultiStop";
import Tile from "./Tile";
import AlgorithmType from "./JourneyForm/AlgorithmType";
import {
  overwriteUserInputStops,
  overwriteUserInputType,
  selectUserInput,
  overwriteUserInputAlgorithm,
} from "../reducers/route";

function JourneyForm() {
  const userInput = useSelector(selectUserInput);
  const dispatch = useDispatch();
  let history = useHistory();

  const handleType = (event) => {
    dispatch(overwriteUserInputType(event.target.value));
  };

  const handleStops = (newStops) => {
    dispatch(overwriteUserInputStops(newStops));
  };

  const handleAlgorithm = (event) => {
    dispatch(overwriteUserInputAlgorithm(event.target.value));
  };

  const handleSubmit = () => {
    history.push("/route");
  };

  return (
    <Tile>
      <Type value={userInput.type} onChange={handleType} />
      {userInput.type === "Single" ? (
        <Single data={userInput.stops} handleChange={handleStops} />
      ) : userInput.type === "Return" ? (
        <Return data={userInput.stops} handleChange={handleStops} />
      ) : (
        <MultiStop data={userInput.stops} handleChange={handleStops} />
      )}
      <AlgorithmType value={userInput.algorithm} onChange={handleAlgorithm} />
      <Button variant="contained" color="primary" onClick={handleSubmit}>
        Find Route
      </Button>
    </Tile>
  );
}

export default JourneyForm;
