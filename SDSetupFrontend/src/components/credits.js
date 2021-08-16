import React from 'react';
import { ButtonGroup, Pagination, SafeAnchor, Button, Table } from 'react-bootstrap';

import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

const PACKAGES_PER_PAGE = 15;

export default class Credits extends React.Component {
    constructor(props) {
        super(props);

        let paginatedPackages = [];
        let allPackages = Object.values(sdsetup.getCreditsPackages());
        allPackages.sort((a, b) => {
            if(a.displayName < b.displayName) { return -1; }
            if(a.displayName > b.displayName) { return 1; }
            return 0;
        });

        for (let i = 0; i < Math.ceil(allPackages.length / PACKAGES_PER_PAGE); i++) {
            let index = i * PACKAGES_PER_PAGE;
            paginatedPackages.push(allPackages.slice(index, index + PACKAGES_PER_PAGE));
        }

        this.state = {
            active: 0,
            paginatedPackages: paginatedPackages
        }
    }

    setPage(i) {
        this.setState({active: i});
    }

    incrementPage(offset) {
        const active = this.state.active;
        const newActive = active + offset;
        if (newActive >= 0 && newActive < this.state.paginatedPackages.length) {
            this.setState({active: this.state.active + offset});
        }
    }

    render() {
        const active = this.state.active;
        const paginatedPackages = this.state.paginatedPackages;
        const packages = paginatedPackages[active];
        const paginationButtons = [];
        const tableItems = [];

        paginationButtons.push(<Pagination.Item key="<" active={false} onClick={() => { this.incrementPage(-1); }}>&lt;</Pagination.Item>);
        for (let i = 0; i < paginatedPackages.length; i++) {
            paginationButtons.push(
                <Pagination.Item key={i} active={i === active} onClick={() => { this.setPage(i); }}>
                    {i+1}
                </Pagination.Item>
            );
        }
        paginationButtons.push(<Pagination.Item key=">" active={false} onClick={() => { this.incrementPage(1); }}>&gt;</Pagination.Item>);

        for (let pkg of packages) {
            tableItems.push(
                <tr>
                    <td>{pkg.displayName}</td>
                    <td>{pkg.authors}</td>
                    <td>{pkg.versionInfo.version}</td>
                    <td className="center">
                        <ButtonGroup size="sm">
                            <Button variant="info" as={SafeAnchor} href={pkg.source}>Source</Button>
                            <Button variant="success" as={SafeAnchor} href={pkg.license}>License</Button>
                        </ButtonGroup>
                    </td>
                </tr>
            );
        }

        return (
            <div>
                <h1 className="tall-margin center">
                    Credits
                </h1>

                <div>
                    <h4>SDSetup Credits</h4>
                    <ul>
                        <li>tomGER, NicholeMattera, Emily, friedkeenan, Shadow, shchmue, WerWolv and elise for the original foundation behind the SDSetup bundle.</li>
                        <li>ForTheUsers for letting me pull certain packages from their Appstore server.</li>
                        <li>Team Neptune for volunteering countless hours to perform end user support.</li>
                        <li>All of our Patrons and donators who help to keep SDSetup costs away from my wallet.</li>
                    </ul>
                </div>

                <div>
                    <h4>Package Credits</h4>
                    <Table striped bordered hover size="sm" responsive="sm">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Authors</th>
                                <th>Version</th>
                                <th>Links</th>
                            </tr>
                        </thead>
                        {tableItems}
                    </Table>
                    <div class="vertical-center-children"><Pagination>{paginationButtons}</Pagination></div>
                </div>
            </div>
        );
    }
}