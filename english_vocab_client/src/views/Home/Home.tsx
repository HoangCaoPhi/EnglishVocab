import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

export default function Home() {
    let navigate = useNavigate();
    return (
        <div className='home'>
            <div className='menu'>
                <Box sx={{ flexGrow: 1 }}>
                    <AppBar position="static">
                        <Toolbar>
                            <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                                English Vocab
                            </Typography>
                            <Button color="inherit"
                                onClick={() => {
                                    navigate(`/sign-up`)
                                }}>Đăng nhập</Button>
                            <Button sx={{
                                backgroundColor: 'success.main',
                                borderRadius: 4,
                                ":hover": {
                                    backgroundColor: 'success.main',
                                    boxShadow: "none"
                                }
                            }}
                                variant="contained"
                                onClick={() => {
                                    navigate(`/sign-in`)
                                }}>Đăng ký</Button>
                        </Toolbar>
                    </AppBar>
                </Box>
            </div>

            <div className='introduce'>

            </div>
        </div>
    );
}
