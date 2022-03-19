<template>
  <v-app>
    <v-main>
      <v-card elevation="0" class="card-center fill-height d-flex flex-column justify-center" :max-width="$vuetify.breakpoint.smAndDown ? '' : '480px'">
        <v-form ref="form" v-model="isValid">
          <v-card-text class="d-flex flex-column">
            <v-text-field
              v-model="username"
              :rules="[generalRules.required]"
              label="Enter your username"
            >
              <v-icon slot="prepend" color="primary">mdi-account</v-icon>
            </v-text-field>

            <v-text-field
              v-model="password"
              :append-icon="passwordShow ? 'mdi-eye' : 'mdi-eye-off'"
              :rules="[generalRules.required, generalRules.min]"
              :type="passwordShow ? 'text' : 'password'"
              label="Enter your password"
              hint="At least 8 characters"
              counter
              @click:append="passwordShow = !passwordShow"
            >
              <v-icon slot="prepend" color="primary">mdi-lock</v-icon>
            </v-text-field>

            <div class="d-flex mt-4">
              <v-spacer></v-spacer>
              <v-btn color="primary" @click="validate">Login</v-btn>
              <v-spacer></v-spacer>
            </div>
          </v-card-text>
        </v-form>
      </v-card>
    </v-main>
  </v-app>
</template>

<style>
.card-center {
  margin-left: auto;
  margin-right: auto;
}
</style>

<script>
export default {
  data: function () {
    return {
      isValid: true,

      username: '',

      password: '',
      passwordShow: false,

      generalRules: {
        required: value => !!value || 'Required',
        min: value => value.length >= 8 || 'Min 8 characters',
      }
    }
  },
  methods: {
    validate() {
      if (this.$refs.form.validate()) {
        const payload = {
          username: this.username,
          password: this.password,
        }
        console.log(payload);
        this.$store.dispatch("account/login", payload);
      }
    }
  }
}
</script>
