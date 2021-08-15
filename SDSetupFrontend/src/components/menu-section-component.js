import React from 'react';
import { Card, Nav } from 'react-bootstrap';
import { FaRegSquare, FaRegCheckSquare } from 'react-icons/fa'

import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

export default class MenuSectionComponent extends React.Component {

    /**
     * @typedef {{
     * section: Section
     * }} Props
     */

    constructor(props) {
        super(props);

        /** @type {Props} */
        this.props = this.props || {};
    }

    render() {
        const section = this.props.section;
        const packages = sdsetup.getVisiblePackagesForSection(section);
        const selections = packages.map((pkg) => {
            let checkbox;
            if (pkg.checked) {
                checkbox = <FaRegCheckSquare className="huge-text"/>;
            } else {
                checkbox = <FaRegSquare className="huge-text"/>;
            }
            return (
                <Nav.Item className="package-selector" key={pkg.id}>
                    <Nav.Link active={pkg.checked} onClick={() => { sdsetup.selectPackage(pkg.id, !pkg.checked); }} className="vertical-center-children">
                        {checkbox}
                        <span>
                            <div>{pkg.displayName}</div>
                            <div className="muted">({pkg.versionInfo.version})</div>
                        </span>
                    </Nav.Link>
                </Nav.Item>
            );
        });

        return (
            <Card>
                <Card.Body>
                    <Card.Title>{section.name}</Card.Title>
                    
                    <Nav justify variant="pills">
                        {selections}
                    </Nav>
                    
                </Card.Body>
            </Card>
        )
    }
}