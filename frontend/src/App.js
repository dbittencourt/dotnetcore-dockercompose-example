import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import PeopleSearch from './components/PeopleSearch';
import Task from './components/Task';

class App extends Component {

  state = {
    people: []
  };

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">Example app</h1>
        </header>

        <p>This app was made using react for the frontend and .net core 2.1 for the backend.</p>
        <p>The backend api can be accessed through <a href='http://localhost:5000/swagger'>http://localhost:5000/swagger</a>.
        As said before, the api uses .net core 2.1, entity framework core with sqlite and a redis cache.</p>


        <CodeTest />
        <br/><br/>
        <p className="App-intro">
          You can search people by their names using the people api:
        </p>

        <PeopleSearch />
      </div>
    );
  }
}

export default App;
