<template>
    <v-container
        class="fill-height"
        fluid
    >
        <v-row
        align="center"
        justify="center"
        class="flex-column"
        >
            <v-col
                cols="12"
                sm="8"
                md="4"
            >
                <v-card class="elevation-12">
                    <v-toolbar
                        color="primary"
                        dark
                        flat
                    >
                        <v-toolbar-title id="login-header" class="text-h5">Login</v-toolbar-title>
                    </v-toolbar>
                    <v-card-text>
                        <v-form>
                        <v-text-field
                            label="Username"
                            name="username"
                            prepend-icon="mdi-account"
                            type="text"
                            v-model="username"
                        ></v-text-field>

                        <v-text-field
                            id="password"
                            label="Password"
                            name="password"
                            prepend-icon="mdi-lock"
                            type="password"
                            v-model="password"
                        ></v-text-field>
                        </v-form>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="primary" v-on:click="doLogin">Login</v-btn>
                    </v-card-actions>
                </v-card>
            </v-col>
            <v-col cols="12"
                sm="8"
                md="4">
                <v-alert v-model="isShowError" outlined dismissible type="error" >
                    {{ errorMessage }}
                </v-alert>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
export default {
    data : function(){
        return {
            username : "",
            password : "",
            errorMessage: ""
        }
    },
    methods: {
        doLogin : function(){
            this.errorMessage = '';

            const param = {
                username : this.username,
                password : this.password,
                grant_type : 'password'
            };

            const that = this;
            this.$axios({
                method : 'post',
                url : 'token',
                data : toQueryString(param)
            }).then(res => {
                localStorage.setItem("access_key", res.data.access_token);
                that.$axios.defaults.headers.common['Authorization'] = 'Bearer ' + res.data.access_token;

                this.$router.push({ name : 'customer-orders'});
            }).catch(err => {
                if(err.response.status == 400){
                    that.errorMessage = err.response.data.error_description;
                }
            })
        }
    },
    computed: {
        isShowError(){
            return !!this.errorMessage;
        }
    }
}

function toQueryString(data){
    return Object.keys(data).map(function(key) {
        return encodeURIComponent(key) + '=' + encodeURIComponent(data[key])
    }).join('&')
}
</script>