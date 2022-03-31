import React from 'react';
import classes from './Tile.module.css';

import {
    Grid,
    Paper
  } from "@material-ui/core";

function Tile(props) {
    return(
        <Grid item xs={12} sm={10} md={5} xl={4}>
        <Paper variant="outlined" className={classes.form}>
            {props.children}
        </Paper>
      </Grid>
    )
}

export default Tile;
