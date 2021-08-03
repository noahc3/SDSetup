import React from 'react';
import { Card, Button, ButtonGroup } from 'react-bootstrap';
import { FiDownload } from "react-icons/fi"

export default class PreconfiguredBundleCard extends React.Component {
    render() {
        const sdsetup = this.props.sdsetup;
        const bundle = this.props.bundle;
        return (
            <Card className="bundle-card">
                <Card.Body>
                    <h6>{bundle.name}</h6>
                    <Card.Text>{bundle.description}</Card.Text>
                </Card.Body>
                <Card.Footer>
                    <ButtonGroup className="btn-group-wide" >
                        <Button variant="danger" className="btn-focus" onClick={() => {sdsetup.selectPackages(bundle.packages)}}>Select and Customize</Button>
                        <Button variant="primary" title="Direct download this bundle as-is, no wait time!"><FiDownload/></Button>
                    </ButtonGroup>
                </Card.Footer>
            </Card>
        )
    }
}