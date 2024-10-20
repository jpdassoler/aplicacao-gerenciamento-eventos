import axios from 'axios';

let apiUrl = '';

export const fetchConfig = async () => {
    const configFileName = process.env.NODE_ENV === 'production' ? 'config.json' : 'config-dev.json';

    try {
        const response = await axios.get(`${configFileName}`);
        console.log('Config carregada:', response.data);
        apiUrl = response.data.REACT_APP_API_URL;
    } catch (error) {
        console.error('Erro ao carregar configuração:', error);
    }
};

export const getApiUrl = () => apiUrl;