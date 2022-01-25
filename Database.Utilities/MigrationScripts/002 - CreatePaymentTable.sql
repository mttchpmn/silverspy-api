CREATE TABLE payment
(
    id             SERIAL PRIMARY KEY,
    auth_id        VARCHAR(255) NOT NULL,
    reference_date timestamptz  NOT NULL,
    type           integer      NOT NULL,
    frequency      integer      NOT NULL,
    value          money        NOT NULL,
    name           VARCHAR(255),
    category       VARCHAR(255),
    details        VARCHAR(255)
);
