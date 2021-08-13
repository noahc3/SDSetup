import React from 'react';
import { Alert, Card, CardGroup, Button } from 'react-bootstrap';
import PreconfiguredBundleCard from './preconfigured-bundle-card';
import CheckboxSectionComponent from './checkbox-section-component';
import parse from 'html-react-parser';

import * as utils from '../utils';
import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';



export default class Console extends React.Component {

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
        const platformid = this.props.platformid;
        const isBundling = sdsetup.isBundling();
        const copyright = sdsetup.getCopyrightText();
        const platform = sdsetup.getPlatformById(platformid);

        const bundles = sdsetup.getBundlesForPlatform(platformid).map((bundle) => {
            return (
                <PreconfiguredBundleCard key={"bundle_"+bundle.name} bundle={bundle} />
            );
        });

        const sections = Object.keys(platform.packageSections).map((id) => {
            const section = platform.packageSections[id];
            if (sdsetup.canShow(section)) {
                return (
                    <div key={"section_"+section.id}>
                        <CheckboxSectionComponent section={section} />
                        <br />
                    </div>
                )
            } else return null;
        });

        return (
            <div>
                <h1 className="tall-margin center">
                    {platform.name}
                </h1>
                <Alert key={platformid+"_topalert"} variant="primary" className="center">
                    Want to hack your Switch but not sure what to do? 
                    Check out the official SDSetup guide: <a href="https://switch.homebrew.guide">https://switch.homebrew.guide</a>
                </Alert>
                <Card>
                    <Card.Body>
                        <Card.Title>Pre-configured bundles</Card.Title>
                        <CardGroup>
                            {bundles}
                        </CardGroup>
                    </Card.Body>
                </Card>
                <br />
                {sections}
                <Button onClick={() => { sdsetup.requestBundle(platformid); }} variant="danger" size="lg" block>Get My Bundle</Button>
                <br />
                <div className="copyright">
                    {parse(copyright)}
                </div>
            </div>
        );
    }
}