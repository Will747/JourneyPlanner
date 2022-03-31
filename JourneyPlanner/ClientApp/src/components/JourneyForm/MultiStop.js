import React from "react";

import classes from "../JourneyForm.module.css";

import { Button } from "@material-ui/core";

import Stop from "./Stop";

function MultiStop({ data, handleChange }) {
  const handleAdd = () => {
    handleChange([...data, null]);
  };

  const handleRemove = () => {
    handleChange(data.slice(0, data.length - 1));
  };

  const handleStopChange = (index, newValue) => {
      handleChange([
        ...data.slice(0, index),
        newValue,
        ...data.slice(index + 1),
      ]);
  };

  return (
    <div className={classes.ToFrom}>
      {data.map((value, index) => (
        <Stop key={index} id={index} value={value} handleChange={handleStopChange} />
      ))}
      <div className={classes.MultiStopButtons}>
        <Button
          variant="contained"
          onClick={handleAdd}
          disabled={data.length >= 7}
        >
          Add Stop
        </Button>
        <Button
          variant="contained"
          onClick={handleRemove}
          disabled={data.length <= 2}
        >
          Remove Stop
        </Button>
      </div>
    </div>
  );
}

export default MultiStop;
