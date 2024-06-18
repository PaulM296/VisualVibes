import React from 'react';
import { Button } from '@mui/material';
import './Pagination.css';

interface PaginationProps {
  pageIndex: number;
  totalPages: number;
  setPageIndex: (pageIndex: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({ pageIndex, totalPages, setPageIndex }) => {
  return (
    <div className="pagination">
      <Button
        disabled={pageIndex === 1}
        onClick={() => setPageIndex(pageIndex - 1)}
      >
        Previous
      </Button>
      <span>{pageIndex} / {totalPages}</span>
      <Button
        disabled={pageIndex === totalPages}
        onClick={() => setPageIndex(pageIndex + 1)}
      >
        Next
      </Button>
    </div>
  );
};

export default Pagination;