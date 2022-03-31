import React, { useState } from "react";

import classes from "../JourneyForm.module.css";

import search from "./search";

import { TextField } from "@material-ui/core";
import { Autocomplete } from "@material-ui/lab";

function ToFrom({ handleToChange, handleFromChange, To, From }) {
  const [optionsTo, setOptionsTo] = useState([]);
  const [optionsFrom, setOptionsFrom] = useState([]);
  const [InputTo, setInputTo] = useState("");
  const [InputFrom, setInputFrom] = useState("");

  return (
    <div className={classes.ToFrom}>
      <Autocomplete
        options={optionsTo}
        getOptionLabel={(option) => option.name + ": " + option.codeName}
        inputValue={InputTo}
        value={To}
        onChange={(event, newValue) => {
          handleToChange(newValue);
        }}
        onInputChange={(event, newValue) => {
          setInputTo(newValue);
          search(newValue, setOptionsTo);
        }}
        renderInput={(params) => (
          <TextField {...params} label="From" variant="outlined" />
        )}
      />

      <Autocomplete
        options={optionsFrom}
        getOptionLabel={(option) => option.name + ": " + option.codeName}
        onChange={(event, newValue) => {
          handleFromChange(newValue);
        }}
        onInputChange={(event, newValue) => {
          setInputFrom(newValue);
          search(newValue, setOptionsFrom);
        }}
        inputValue={InputFrom}
        value={From}
        renderInput={(params) => (
          <TextField {...params} label="To" variant="outlined" />
        )}
      />
    </div>
  );
}

export default ToFrom;
