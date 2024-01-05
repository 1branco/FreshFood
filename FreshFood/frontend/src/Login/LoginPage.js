import { Client as Styletron } from "styletron-engine-monolithic";
import { Provider as StyletronProvider } from "styletron-react";
import { LightTheme, BaseProvider, styled } from "baseui";
import * as React from "react";
import { FormControl } from "baseui/form-control";
import { Input } from "baseui/input";
import { validate as validateEmail } from "email-validator"; // add this package to your repo: `$ pnpm add email-validator`
import { useStyletron } from "baseui";
import { Alert } from "baseui/icon";
import { Button } from "baseui/button";
import { Card, StyledBody, StyledAction } from "baseui/card";

const engine = new Styletron();

const Centered = styled("div", {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
  height: "100%",
});

function Negative() {
  const [css, theme] = useStyletron();
  return (
    <div
      className={css({
        display: "flex",
        alignItems: "center",
        paddingRight: theme.sizing.scale500,
        color: theme.colors.negative400,
      })}
    >
      <Alert size="18px" />
    </div>
  );
}

export default function Home() {
  const [email, setEmail] = React.useState("");
  const [password, setPassword] = React.useState("");

  const [isValid, setIsValid] = React.useState(false);
  const [isVisited, setIsVisited] = React.useState(false);
  const shouldShowError = !isValid && isVisited;

  const onSubmit = (event) => {
    alert(email + "\n" + password);

    // clears all input values in the form
    setEmail("");
    setPassword("");
  };

  const onChangeEmail = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { value } = event.currentTarget;
    setIsValid(validateEmail(value));
    setEmail(value);
  };

  return (
    <StyletronProvider value={engine}>
      <BaseProvider theme={LightTheme}>
        <Centered>
          <Card>
            <StyledBody>
              <FormControl
                label="Your email"
                error={
                  shouldShowError ? "Please input a valid email address" : null
                }
              >
                <Input
                  id="input-id"
                  value={email}
                  placeholder="Email"
                  onChange={onChangeEmail}
                  onBlur={() => setIsVisited(true)}
                  error={shouldShowError}
                  overrides={shouldShowError ? { After: Negative } : {}}
                />
              </FormControl>
              <FormControl label="Your Password">
                <Input
                  id="password-id"
                  type="password"
                  placeholder="Password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
              </FormControl>
            </StyledBody>
            <StyledAction>
              <FormControl>
                <Button
                  overrides={{
                    BaseButton: {
                      style: {
                        width: "100%",
                      },
                    },
                  }}
                  type="submit"
                  onClick={onSubmit}
                >
                  Login
                </Button>
              </FormControl>
            </StyledAction>
          </Card>
        </Centered>
      </BaseProvider>
    </StyletronProvider>
  );
}
