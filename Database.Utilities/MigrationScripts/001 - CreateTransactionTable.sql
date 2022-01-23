CREATE TABLE transaction
(
    id               SERIAL PRIMARY KEY,
    auth_id        VARCHAR(255) NOT NULL,
    transaction_id   VARCHAR(255) NOT NULL,
    transaction_date timestamptz NOT NULL,
    processed_date   timestamptz NOT NULL,
    reference        VARCHAR(255) NOT NULL,
    description      VARCHAR(255) NOT NULL,
    type             integer NOT NULL,
    value            money NOT NULL,
    category         VARCHAR(255),
    details          VARCHAR(255),
    UNIQUE (auth_id, transaction_id)
);
