export const search = (name, cb) => {
    return fetch(`http://localhost:5000/api/people?q=${name}`)
    .then(this.checkStatus)
    .then(response => response.json())
    .then(cb)
    .catch(error => {});
}

export const getAllPeople = () => {
    return fetch('http://localhost:5000/api/people')
    .then(this.checkStatus)
    .then(response => response.json());
}

const ckeckStatus = (response) => {
    if (response.status >= 200 && response.status < 300) 
        return response;
    const error = new Error(`HTTP Error ${response.statusText}`);
    error.status = response.statusText;
    error.response = response;
    console.log(error);
    throw error;
}
