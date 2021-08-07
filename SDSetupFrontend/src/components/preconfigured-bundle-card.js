import React from 'react';
import { Card, Button, ButtonGroup } from 'react-bootstrap';
import { FiDownload } from "react-icons/fi"

import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

export default class PreconfiguredBundleCard extends React.Component {

    /**
     * @typedef {{
     * bundle: Bundle
     * }} Props
     */

    constructor(props) {
        super(props);

        /** @type {Props} */
        this.props = this.props || {};
    }

    render() {
        const bundle = this.props.bundle;
        return (
            <Card className="bundle-card">
                <Card.Body>
                    <h6>{bundle.name}</h6>
                    <Card.Text>{bundle.description}</Card.Text>
                </Card.Body>
                <Card.Footer className="seamless">
                    <ButtonGroup className="btn-group-wide" >
                        <Button variant="danger" className="btn-focus" onClick={() => {sdsetup.selectPackages(bundle.packages)}}>Select and Customize</Button>
                        <Button variant="primary" title="Direct download this bundle as-is, no wait time!"><FiDownload/></Button>
                    </ButtonGroup>
                </Card.Footer>
            </Card>
        )
    }
}