<template>
  <v-app>
    <router-view></router-view>
  </v-app>
</template>

<script>

export default {
  name: 'App'
};
</script>

<style>
html {
	scroll-behavior: smooth;
}

.sa-body-text {
  font-family: 'Quicksand';
}

.sa-body-heading {
  font-family: 'Eczar';
}
</style>

<style lang="scss">
  @import '~@sweetalert2/theme-material-ui/material-ui.scss';
</style>