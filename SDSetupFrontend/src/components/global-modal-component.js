import React from 'react';
import { Button, ButtonGroup, Modal, ProgressBar, Form, InputGroup } from 'react-bootstrap';
import { SiPatreon, SiKoFi } from 'react-icons/si';
import { FaRegCopy } from 'react-icons/fa'

import * as utils from '../utils';
import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

/**
 * @typedef {{
 * show: boolean
 * display: string
 * progress: BundlerProgress
 * bundleDownloadUrl: string
 * bundleShareUrl: string
 * }} State
 */

export default class GlobalModal extends React.Component {
    constructor(props) {
        super(props);

        /** @type {State} */
        this.state = {
            show: false,
            display: "bundle-success",
            progress: {},
            bundleDownloadUrl: "",
            bundleShareUrl: ""
        }

        sdsetup.setDefaultBundleProgressHandler((progress) => { this.handleProgress(progress); });
        sdsetup.setDefaultBundleSuccessHandler((url) => { this.handleBundleSuccess(url); })
    }

    handleProgress(prog) {
        this.setState({ show: true, display: "progress", progress: prog });
    }

    /**
     * @param {sdsetup.BundleResult} result 
     */
    handleBundleSuccess(result) {
        window.location.href = result.bundleUrl;
        this.setState({ 
            show: true, 
            display: "bundle-success", 
            bundleDownloadUrl: result.bundleUrl, 
            bundleShareUrl: utils.buildShareString(result.platform, result.packages) });
    }

    closeModal() {
        const display = this.state.display;

        if (display === "bundle-success") {
            this.setState({show: false});
        }
    }

    render() {
        const show = this.state.show;
        const display = this.state.display;
        let modalContents;

        if (display === "progress") {
            modalContents = this.renderBundlerProgress();
        } else if (display === "bundle-success") {
            modalContents = this.renderBundleSuccess();
        } else modalContents = null;

        return (
            <Modal 
            show={show} 
            centered>
                {modalContents}
            </Modal>
        );
    }

    renderBundlerProgress() {
        const progress = this.state.progress;
        return (
            <div>
                <Modal.Header>
                    <Modal.Title>Your bundle is being prepared, please wait.</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className="center">
                        <h5>{progress.currentTask}</h5>
                        <p>({progress.progress}/{progress.total})</p>
                        <ProgressBar variant="success" animated now={progress.progress} max={progress.total} />
                    </div>
                </Modal.Body>
            </div>
        );
    }

    renderBundleSuccess() {
        const bundleUrl = this.state.bundleDownloadUrl;
        const shareUrl = this.state.bundleShareUrl;
        const donateMessage = (
            <div>
                <h5>If you like SDSetup, please consider supporting us by donating.</h5>
                <p>There are a number of hosting costs involved in keeping SDSetup running 
                and we would greatly appreciate it if you could help out!</p>
                <ProgressBar variant="info" animated now={50} max={121} label="$50/$121" />
                <br/>
                <ButtonGroup className="btn-group-wide">
                    <Button variant="warning"><SiPatreon/> Patreon (recurring donation)</Button>
                    <Button><SiKoFi/> Ko-fi (one-time donation)</Button>
                </ButtonGroup>
            </div>
        )

        return (
            <div>
                <Modal.Header>
                    <Modal.Title>Your bundle is downloading!</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div className="center">
                        <p>If your download did not start, please <a href={bundleUrl}>click here</a>. 
                        This link will remain active for one hour.</p>

                        <p>If you want to share your custom bundle with somebody or save it for later, you can use the following link:</p>
                        <InputGroup>
                            <Form.Control type="text" readOnly value={shareUrl} />
                            <InputGroup.Append>
                                <Button 
                                    variant="danger"
                                    onClick={() => { utils.copyTextToClipboard(shareUrl); }}>
                                        <FaRegCopy/>
                                </Button>
                            </InputGroup.Append>
                        </InputGroup>
                        <hr/>
                        <div className="center">
                            {donateMessage}
                        </div>
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="danger" onClick={() => { this.closeModal(); }}>Close</Button>
                </Modal.Footer>
            </div>
        );
    }
}