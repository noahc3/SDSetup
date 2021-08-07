import React from 'react';
import { Alert, Card, CardGroup, Button, Modal, ProgressBar } from 'react-bootstrap';
import PreconfiguredBundleCard from './preconfigured-bundle-card';
import CheckboxSectionComponent from './checkbox-section-component';
import parse from 'html-react-parser';

import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

class DownloadModalContents extends React.Component {
    constructor(props) {
        super(props);
        sdsetup.setModalUpdate(() => { this.forceUpdate(); });
    }

    render() {
        const bundlerProgress = sdsetup.getBundlerProgress();

        return (
                <div>
                    <Modal.Header>
                        <Modal.Title>Your bundle is being prepared, please wait.</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div class="center">
                            <h5>{bundlerProgress.currentTask}</h5>
                            <p>({bundlerProgress.progress}/{bundlerProgress.total})</p>
                            <ProgressBar variant="success" animated now={bundlerProgress.progress} max={bundlerProgress.total} />
                        </div>
                    </Modal.Body>
                </div>
        )
    }
}

class Console extends React.Component {

    /**
     * @typedef {{
     * platformid: string
     * }} Props
     */

    constructor(props) {
        super(props);

        /** @type {Props} */
        this.props = this.props || {};
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

        const downloadingModal = (
            <Modal key="downloading-modal" show={isBundling} size="lg" centered>
                <DownloadModalContents/>
            </Modal>
        );

        return (
            <div>
            {downloadingModal}
                <h1 className="center">
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
                <div class="copyright">
                    {parse(copyright)}
                </div>
            </div>
        );
    }
}

export default Console;