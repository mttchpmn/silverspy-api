CREATE TABLE transaction
(
    id               SERIAL PRIMARY KEY,
    transaction_id   VARCHAR(255) UNIQUE NOT NULL,
    transaction_date timestamptz,
    processed_date   timestamptz,
    reference        VARCHAR(255),
    particulars      VARCHAR(255),
    type             integer,
    value            money,
    category         VARCHAR(255),
    details          VARCHAR(255)
);
