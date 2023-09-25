import React, {useState} from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import {loginUser} from '../api';

export default function LoginBar(props) {
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

    const widthStyle = {
        width: '40%'
    }

    return (
        <div className="login-bar">
            <Form style={widthStyle}>
                <Form.Group className="mb-3" controlId="formPlaintextEmail">
                    <Form.Control type="email" placeholder="name@example.com" onChange={(event) => handleEmailChange(event)}/>
                </Form.Group>
                <Form.Group className="mb-3" controlId="formPlaintextPassword">
                    <Form.Control type="password" placeholder="Password" onChange={(event) => handlePasswordChange(event)}/>
                </Form.Group>
            </Form>
            <Button variant="outline-light" style={widthStyle} onClick={() => handleClick()}>Login</Button>{' '}
        </div>
    )
}