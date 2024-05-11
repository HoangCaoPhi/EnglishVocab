import { useState } from 'react';
import { Button, FormControl, TextField, Typography } from "@mui/material";
import LoadingButton from '@mui/lab/LoadingButton';
import './SignIn.scss';

const SignInSide = () => {
    const [loading, setLoading] = useState(false);
    const [formValues, setFormValues] = useState({
        firstName: '',
        lastName: '',
        username: '',
        email:"",
        password: '',
        confirmPassword: ''
    });

    const handleSubmit = (event: { preventDefault: () => void; }) => {
        event.preventDefault();
        setLoading(true);


        // Simulate API call
        setTimeout(() => {
            setLoading(false);
        }, 2000); // Simulated duration of API call
    };

    const handleInputChange = (event: { target: { name: any; value: any; }; }) => {
        const { name, value } = event.target;
        setFormValues({ ...formValues, [name]: value });
    };

    return (
        <form noValidate onSubmit={handleSubmit} className="sign-in">
            <div className="si-wrap">
                <div className="si-title">
                    <Typography component="h1" variant="h5" className="text-1xl font-bold text-blue-700 mb-2">
                        Đăng ký tài khoản
                    </Typography>
                </div>
                <div className="si-form">
                    <FormControl fullWidth >
                        <div className="si-label si-name-user mt-2 mb-2">
                            <TextField
                                className="mr-2"
                                id="firstName"
                                name="firstName"
                                label="Họ"
                                variant="outlined"
                                required 
                                onChange={handleInputChange}
                                value={formValues.firstName}
                            />
                            <TextField
                                id="lastName"
                                name="lastName"
                                label="Tên"
                                variant="outlined"
                                required 
                                onChange={handleInputChange}
                                value={formValues.lastName}
                            />
                        </div>

                        <div className="si-label mt-2 mb-2">
                            <TextField
                                className="w-full"
                                id="username"
                                name="username"
                                label="Tên người dùng"
                                variant="outlined"
                                required 
                                onChange={handleInputChange}
                                value={formValues.username}
                            />
                        </div>

                        <div className="si-label mt-2 mb-2">
                            <TextField
                                className="w-full"
                                id="email"
                                name="email"
                                label="Email"
                                variant="outlined"
                                required 
                                onChange={handleInputChange}
                                value={formValues.email}
                            />
                        </div>

                        <div className="si-label mt-2 mb-2">
                            <TextField
                                className="w-full"
                                id="password"
                                name="password"
                                label="Mật khẩu"
                                variant="outlined"
                                required 
                                type="password"
                                onChange={handleInputChange}
                                value={formValues.password}
                            />
                        </div>

                        <div className="si-label mt-2 mb-2">
                            <TextField
                                className="w-full"
                                id="confirmPassword"
                                name="confirmPassword"
                                label="Xác nhận mật khẩu"
                                variant="outlined"
                                required 
                                type="password"
                                onChange={handleInputChange}
                                value={formValues.confirmPassword}
                            />
                        </div>

                        <LoadingButton
                            loading={loading}
                            variant="contained"
                            className="mt-2"
                            type="submit"
                        >
                            Đăng ký
                        </LoadingButton>
                    </FormControl>
                </div>
            </div>
        </form>
    );
};

export default SignInSide;
