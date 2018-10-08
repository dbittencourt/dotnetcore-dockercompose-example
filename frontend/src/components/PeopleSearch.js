import React, { Component } from 'react';
import { search } from '../utils/search';
import '../App.css';

export default class PeopleSearch extends Component {
    
    constructor(props){
      super(props);
      this.state = {
        people: [],
        searchValue: ""
      };

      this.handleSearchChange = this.handleSearchChange.bind(this);
    }
    
    handleSearchChange(event){
        const value = event.target.value;
    
        this.setState({
          searchValue: value
        });
    
        if (value === "") {
          this.setState({
            people: []
          });
        } 
    
        search(value, people => {
            this.setState({
                people: people
            });
        });
    }

    showPets(person){

    }
    
    render() {
        const { people } = this.state;
    
        const peopleRows = people.map((person, idx) => (
          <tr key={idx} onClick={() => this.showPets(person)}>
            <td>{person.name}</td>
            <td className="right aligned">{person.age}</td>
            <td className="right aligned">{person.gender}</td>
            <td className="right aligned">Show pets</td>
          </tr>
        ));
    
        return (
          <div id="people-search">
            <table className="ui selectable structured large table">
              <thead>
                <tr>
                  <th colSpan="5">
                    <div className="ui fluid search">
                      <div className="ui icon input">
                        <input
                          className="prompt"
                          type="text"
                          placeholder="Search people..."
                          value={this.state.searchValue}
                          onChange={this.handleSearchChange}
                        />
                        <i className="search icon" />
                      </div>
                    </div>
                  </th>
                </tr>
                <tr>
                  <th className="eight wide">Name</th>
                  <th>Age</th>
                  <th>Gender</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {peopleRows}
              </tbody>
            </table>
          </div>
        );
    }
}