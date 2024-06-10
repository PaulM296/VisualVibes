export interface PaginationResponse<T> {
    items: T[];
    pageIndex: number;
    totalPages: number;
}

export interface PaginationRequestDto {
    pageIndex: number;
    pageSize: number;
}