import React from 'react';
import { Card } from 'react-bootstrap';
import SubcategoryComponent from './subcategory-component';

import * as sdsetup from '../sdsetup-api';
import '../sdsetup-typedef';

export default class CategoryComponent extends React.Component {

    /**
     * @typedef {{
     * category: Category
     * }} Props
     */

    constructor(props) {
        super(props);

        /** @type {Props} */
        this.props = this.props || {};
    }

    render() {
        const category = this.props.category;
        const subcategories = Object.keys(category.subcategories).map((key) => {
            const subcategory = category.subcategories[key];
            if (sdsetup.canShow(subcategory)) {
                return (
                    <SubcategoryComponent key={"subcat_"+subcategory.id} subcategory={subcategory}/>
                )
            } else return null;
        });
        if (subcategories.length > 0) {
            return (
                <Card>
                    <Card.Body>
                        <Card.Title>{category.name}</Card.Title>
                        {subcategories}
                    </Card.Body>
                </Card>
            )
        } else {
            return null;
        }
    }
}