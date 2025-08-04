import { api } from 'boot/axios';

export const useExpenseApi = () => {
  const createExpense = async (payload: {
    amount: number;
    date: string;
    categoryId: number;
    tags: string[];
    note: string;
  }) => {
    const res = await api.post('/expense', payload);
    return res.data;
  };

  return { createExpense };
};
