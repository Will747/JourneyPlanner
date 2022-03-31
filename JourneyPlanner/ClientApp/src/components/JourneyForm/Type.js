import React from 'react';
import {FormControlLabel, Radio, RadioGroup} from "@material-ui/core";
import classes from "../JourneyForm.module.css";

function Type(props) {
    return (
        <RadioGroup
            className={classes.ratios}
            row
            value={props.value}
            onChange={props.onChange}
        >
            <FormControlLabel
                labelPlacement="bottom"
                value="Single"
                control={<Radio />}
                label="Single"
            />
            <FormControlLabel
                labelPlacement="bottom"
                value="Return"
                control={<Radio />}
                label="Return"
            />
            <FormControlLabel
                value="Multi-Stop"
                control={<Radio />}
                label="Multi-Stop"
                labelPlacement="bottom"
            />
        </RadioGroup>
    );
}

export default Type;
