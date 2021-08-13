import React from 'react';
import { Redirect } from "react-router-dom";

import * as utils from '../utils';
import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

export default class Share extends React.Component {

    /**
     * @typedef {{
     * platformid: string
     * }} Props
     */

    constructor(props) {
        super(props);

        /** @type {Props} */
        this.props = this.props || {};

        let prePackages = utils.getPackagesFromQuery();
        if (prePackages.length > 0) {
            sdsetup.selectPackages(prePackages);
        }
    }

    render() {
        const platform = this.props.platformid;
        return <Redirect to={"/console/" + platform} />
    }
}