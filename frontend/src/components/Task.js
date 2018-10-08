import React, { Component } from 'react';
import { getAllPeople } from '../utils/search';
import '../App.css';

export default class Task extends Component {
    constructor(props){
        super(props);
        
        this.state = {
            people: []
        };

        this.printChallengeResult = this.printChallengeResult.bind(this);
    }

    componentDidMount() {
        getAllPeople()
        .then(people => {
            this.setState({
                people: people
            });
        });
    }

    getCatOwners(){
        var catOwners = this.state.people.filter(person => person.pets.some(pet => pet.type === 'Cat'));
        return catOwners;
    }

    getMales(people){
        var males = people.filter(person => person.gender === 'Male');
        return males;
    }

    getFemales(people){
        var females = people.filter(person => person.gender === 'Female');
        return females;
    }

    getCatNames(people){
        var cats = people.map(person => person.pets.filter(pet => pet.type === 'Cat'));
        var names = [].concat.apply([], cats).map(cat => cat.name).sort();

        return names;    
    }

    createList(type, list){
        return (
            <div className='list'>
                <h4>{type}</h4>
                <ul>
                    {list.map((item,i) => <li key={i}>{item}</li>)}
                </ul>
            </div>
        );
    }

    printChallengeResult(){
        var catOwners = this.getCatOwners();
        var males = this.getCatNames(this.getMales(catOwners));
        var females = this.getCatNames(this.getFemales(catOwners));

        return (<div>
            {this.createList('Male', males)}
            {this.createList('Female', females)}
        </div>);
    }

    render(){
        return (
            <div>
                <p>Challenge: Retrieve all people and output a list of all cats in alphabetical order under a heading of 
                    the gender of their owner.</p>

                {this.printChallengeResult()}

            </div>
        );
    }
}