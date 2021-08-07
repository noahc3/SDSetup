import React from 'react';
import { Form } from 'react-bootstrap';
import PackageCheckbox from './package-checkbox';

import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

export default class SubcategoryComponent extends React.Component {

    /**
     * @typedef {{
     * subcategory: Subcategory
     * }} Props
     */

    constructor(props) {
        super(props);

        /** @type {Props} */
        this.props = this.props || {};
    }

    render() {
        const subcategory = this.props.subcategory;
        const packages = subcategory.packages.map((id) => {
            const pkg = sdsetup.getPackageById(id);
            if (sdsetup.canShow(pkg)) {
                return (
                    <PackageCheckbox key={"pkg_"+pkg.id} pkg={pkg} />
                )
            } else return null;
        });
        return (
            <div className="subcategory">
                <h6 className="muted">{subcategory.name}</h6>
                <Form>
                {packages}
                </Form>
            </div>
        )
    }
}