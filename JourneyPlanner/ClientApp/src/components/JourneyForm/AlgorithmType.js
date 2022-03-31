import React from 'react';
import {FormControlLabel, Radio, RadioGroup} from "@material-ui/core";
import classes from "../JourneyForm.module.css";

function AlgorithmType(props) {
    return (
        <RadioGroup
            className={classes.ratios}
            row
            value={props.value}
            onChange={props.onChange}
        >
            <FormControlLabel
                value="Distance"
                control={<Radio />}
                label="Distance"
            />
            <FormControlLabel
                disabled
                value="Timetable"
                control={<Radio />}
                label="Timetable"
            />
        </RadioGroup>
    );
}

export default AlgorithmType;
