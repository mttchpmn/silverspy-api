CREATE TABLE transaction
(
    id               SERIAL PRIMARY KEY,
    auth_id        VARCHAR(255),
    transaction_id   VARCHAR(255) UNIQUE NOT NULL,
    transaction_date timestamptz,
    processed_date   timestamptz,
    reference        VARCHAR(255),
    description      VARCHAR(255),
    type             integer,
    value            money,
    category         VARCHAR(255),
    details          VARCHAR(255)
);
