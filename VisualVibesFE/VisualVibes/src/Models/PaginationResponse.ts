export interface PaginationResponse<T> {
    items: T[];
    pageIndex: number;
    totalPages: number;
}