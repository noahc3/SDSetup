import React from 'react';
import { Form, OverlayTrigger, Popover } from 'react-bootstrap';

export default class PackageCheckbox extends React.Component {

    /**
     * @typedef {{
     * pkg: Package
     * }} Props
     */

    constructor(props) {
        super(props);

        /** @type {Props} */
        this.props = this.props || {};
    }

    render() {
        const sdsetup = this.props.sdsetup;
        const pkg = this.props.pkg;
        const popover = (
            <Popover id={"popover_"+pkg.id}>
                <Popover.Title as="h3">{pkg.name} <small><br/>by {pkg.authors}</small></Popover.Title>
                <Popover.Content>
                    <div></div>
                    <div>{pkg.description}</div>
                    <br/>
                    <div>License: {pkg.license}</div>
                </Popover.Content>
            </Popover>
        );
        return (
            <div className="package-checkbox">
                <OverlayTrigger 
                    key={"overlay_"+pkg.id}
                    placement="right"
                    overlay={popover}
                >
                    <div>
                        <Form.Check id={"cbx_"+pkg.id} type="checkbox">
                            <Form.Check.Input checked={pkg.checked} onChange={(event) => { sdsetup.selectPackage(pkg.id, event.target.checked); }} type="checkbox"/>
                            <Form.Check.Label>{pkg.name} <sub className="muted">({pkg.versionInfo.version})</sub></Form.Check.Label>
                        </Form.Check>
                    </div>
                </OverlayTrigger>
            </div>
        )
    }
}