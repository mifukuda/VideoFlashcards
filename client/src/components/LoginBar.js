import React, {useState} from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import {loginUser} from '../api';
import { Link, useNavigate } from "react-router-dom";

export default function LoginBar(props) {
    const navigate = useNavigate();
    
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    function handleClick() {
        loginUser({
            email: email,
            password: password
        }).then((response) => {
            if(response.status === 200) {
                console.log(response.data);
                props.setToken(response.data.token);
                navigate('/home/');
            }
        }).catch((error) => {
            console.log(error);
        });
    }

    function handleEmailChange(event) {
        setEmail(event.target.value);
    }

    function handlePasswordChange(event) {
        setPassword(event.target.value);
    }

    return (
        <div className="login-bar">
            <img src={require('../images/logo.png')} alt="logo.png" className="login-bar-logo"/>
            <Form style={{ width: '40%'}}>
                <Form.Group className="mb-3" controlId="formPlaintextEmail">
                    <Form.Control type="email" placeholder="name@example.com" onChange={(event) => handleEmailChange(event)}/>
                </Form.Group>
                <Form.Group className="mb-3" controlId="formPlaintextPassword">
                    <Form.Control type="password" placeholder="Password" onChange={(event) => handlePasswordChange(event)}/>
                </Form.Group>
            </Form>
            <Button variant="outline-light" style={{ width: "40%", marginBottom: "2%"}} onClick={() => handleClick()}>Login</Button>{' '}
            <Link to="/register">Create Account</Link>
        </div>
    )
}