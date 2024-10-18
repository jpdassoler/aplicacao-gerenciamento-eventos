import axios from 'axios';

let apiUrl = '';

export const fetchConfig = async () => {
    try {
        const response = await axios.get('/config.json');
        console.log('Config carregada:', response.data);
        apiUrl = response.data.REACT_APP_API_URL;
    } catch (error) {
        console.error('Erro ao carregar configuração:', error);
    }
};

export const getApiUrl = () => apiUrl;