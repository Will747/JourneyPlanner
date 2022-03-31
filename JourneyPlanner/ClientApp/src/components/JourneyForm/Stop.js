import React, { useState } from "react";

import { Autocomplete } from "@material-ui/lab";
import TextField from '@material-ui/core/TextField';

import search from "./search";

function Stop({id, value, handleChange}) {
    const [options, setOptions] = useState([]);
    const [input, setInput] = useState("");

    return (
        <Autocomplete
        options={options}
        getOptionLabel={(option) => option.name + ": " + option.codeName}
        inputValue={input}
        value={value}
        onChange={(event, newValue) => {
          handleChange(id, newValue);
        }}
        onInputChange={(event, newValue) => {
          setInput(newValue);
          search(newValue, setOptions);
        }}
        renderInput={(params) => (
          <TextField {...params} label={"Stop " + (id + 1)} variant="outlined" />
        )}
      />
    );
}

export default Stop;
