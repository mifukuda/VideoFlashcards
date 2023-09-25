import React, {useState} from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import {registerUser} from '../api';
import {useNavigate} from "react-router-dom";

export default function RegistrationScreen() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [password, setPassword] = useState("");
    const [passwordConfirm, setPasswordConfirm] = useState("");

    // Make request to backend to create user in database
    function handleClick(event) {
        registerUser({
            email: email,
            password: password,
            passwordConfirm: passwordConfirm,
            firstName: firstName,
            lastName: lastName
        }).then((response) => {
            if(response.status === 200) {
                console.log(response.data);
                navigate('/home/');
            }
        }).catch((error) => {
            console.log(error);
        });
    }
    
    function handleEmailChange(event) {
        setEmail(event.target.value);
    }

    function handleFirstNameChange(event) {
        setFirstName(event.target.value);
    }

    function handleLastNameChange(event) {
        setLastName(event.target.value);
    }

    function handlePasswordChange(event) {
        setPassword(event.target.value);
    }
    
    function handlePasswordConfirmChange(event) {
        setPasswordConfirm(event.target.value);
    }

    const formStyles = {
        borderColor: "black",
        borderWidth: "1.5px"
    }

    return(
        <div className="registration-screen">
            <div className="registration-form">
                <img src={require('../images/blacklogo.png')} alt="blacklogo.png" className="registration-screen-logo"/>
                <div className="registration-fields">
                    <Form>
                        <Form.Group className="mb-3" controlId="formPlaintextEmail">
                            <Form.Control style={formStyles} type="email" placeholder="Email" onChange={(event) => handleEmailChange(event)}/>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formPlaintextFirstName">
                            <Form.Control style={formStyles} type="text" placeholder="First name" onChange={(event) => handleFirstNameChange(event)}/>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formPlaintextLastName">
                            <Form.Control style={formStyles} type="text" placeholder="Last name" onChange={(event) => handleLastNameChange(event)}/>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formPlaintextPassword">
                            <Form.Control style={formStyles} type="password" placeholder="Password" onChange={(event) => handlePasswordChange(event)}/>
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="formPlaintextPasswordConfirm">
                            <Form.Control style={formStyles} type="password" placeholder="ConfirmPassword" onChange={(event) => handlePasswordConfirmChange(event)}/>
                        </Form.Group>
                    </Form>
                </div>
                <Button variant="dark" style={{ width: "55%", marginBottom: "2%"}} onClick={() => handleClick()}>Register</Button>{' '}
            </div>
        </div>
    )
}